(ns brianium.simple-math.core
  (:require [clojure.spec.alpha :as s]
            [brianium.simple-math.spec :as math])
  (:import (java.text DecimalFormat)))

(defn make-operation
  [operator left right]
  {:operator operator
   :left     left
   :right    right
   :result   (try 
               ((resolve (symbol operator)) left right)
               (catch Exception _
                 0))})

(s/fdef make-operation
  :args (s/cat :operator ::math/operator :left ::math/left :right ::math/right)
  :ret  ::math/operation)

(defn format-decimal
  [f]
  (-> (DecimalFormat. "#.##") 
      (.format f)))

(s/fdef format-decimal
  :args (s/cat :f float?)
  :ret  string?)

(defn calculate
  [left right]
  (->> [#'+ #'- #'* #'/]
       (map #(make-operation % left right))))

(s/fdef calculate
  :args (s/cat :left float? :right float?)
  :ret  (s/coll-of ::math/operation))

(defn render
  [{:keys [operator left right result]}]
  (str
   (format-decimal left) " " 
   (-> operator meta :name) " " 
   (format-decimal right) " = " 
   (format-decimal result)))

(s/fdef render
  :args (s/cat :operation ::math/operation)
  :ret  string?)

(def feedback {:failure  "Value must be a number."
               :negative "Value cannot be negative."
               :empty    "Value cannot be empty."})
