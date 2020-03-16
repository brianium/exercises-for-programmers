(ns brianium.counter.gui
  (:require [cljfx.api :as fx]))

(def *state
  (atom {:input ""}))

(defn input-field
  [{:keys [input]}]
  {:fx/type         :text-field
   :on-text-changed #(swap! *state assoc :input %)
   :text            input})

(defn root
  [{:keys [input]}]
  {:fx/type :stage
   :showing true
   :title   "Counter 5000"
   :scene   {:fx/type :scene
             :root    {:fx/type  :v-box
                       :children [{:fx/type :label
                                   :text    (str "Characters: " (count input)) }
                                  {:fx/type input-field
                                   :input   input}]}}})

(def renderer
  (fx/create-renderer
   :middleware (fx/wrap-map-desc assoc :fx/type root)))

(defn run []
  (fx/mount-renderer *state renderer))
