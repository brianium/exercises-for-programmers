(ns brianium.retirement.input
  (:require [clojure.spec.alpha :as s]
            [brianium.retirement.input.spec :as input]))

(defn make-parse-state
  [i]
  (if (< i 0)
    {:state :negative :value i}
    {:state :success :value i}))

(s/fdef make-parse-state
  :args (s/cat :i int?)
  :ret  ::input/state)

(defn parse
  [s]
  (try
    (make-parse-state (Integer/parseInt s))
    (catch Exception _
      {:state :failure})))

(s/fdef parse
  :args (s/cat :s string?)
  :ret  (s/tuple int? boolean?))

(defn success?
  [parse-state]
  (= :success (:state parse-state)))

(s/fdef success?
  :args (s/cat :state ::input/state)
  :ret  boolean?)

(def feedback {:failure  "Value must be a number."
               :negative "Value cannot be negative."
               :empty    "Value cannot be empty."})
