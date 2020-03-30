(ns brianium.simple-math.gui
  (:require [cljfx.api :as fx]
            [brianium.simple-math.core :as core]
            [brianium.simple-math.input :as input]))

(def *state (atom {:left  (input/make-parse-state 0)
                   :right (input/make-parse-state 0)}))

(defn number-input
  [{:keys [on-change text]}]
  {:fx/type :text-field
   :on-text-changed on-change
   :text text})

(defn feedback
  [{{:keys [state]} :input}]
  (if (#{:negative :empty :failure} state)
    {:fx/type   :label
     :min-width 170
     :style     {:-fx-text-fill :red}
     :text      (core/feedback state)}
    {:fx/type :label
     :min-width 170
     :visible false}))

(defn operation
  [string]
  {:fx/type :label
   :text    string})

(defn root [{:keys [left right]}]
  (let [valid? (every? input/success? [left right])]
    {:fx/type   :stage
     :showing   true
     :title     "Simple Math 5000"
     :width     350
     :resizable false
     :scene     {:fx/type :scene
                 :root    {:fx/type  :v-box
                           :padding  10
                           :children [{:fx/type  :h-box
                                       :spacing  97
                                       :children [{:fx/type :label
                                                   :text    "First number:"}
                                                  {:fx/type :label
                                                   :text    "Second number:"}]}
                                      
                                      {:fx/type   :h-box
                                       :spacing   10
                                       :alignment :center
                                       :children  [{:fx/type   number-input
                                                    :on-change #(swap! *state assoc :left (input/parse %))
                                                    :text      "0"}
                                                   {:fx/type   number-input                                                  
                                                    :on-change #(swap! *state assoc :right (input/parse %))
                                                    :text      "0"}]}
                                      
                                      {:fx/type  :h-box
                                       :children [{:fx/type feedback
                                                   :input   left}
                                                  {:fx/type feedback
                                                   :input   right}]}
                                      
                                      {:fx/type  :v-box
                                       :padding  {:top 10}
                                       :visible  valid?
                                       :children (if valid?
                                                   (->> (mapv :value [left right])
                                                        (apply core/calculate)
                                                        (mapv core/render)
                                                        (mapv operation))
                                                   [])}]}}}))

(def renderer
  (fx/create-renderer
   :middleware (fx/wrap-map-desc assoc :fx/type root)))

(defn run []
  (fx/mount-renderer *state renderer))
